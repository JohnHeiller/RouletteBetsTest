using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RouletteBets_WebApp.DataAccess;
using RouletteBets_WebApp.DTO;
using RouletteBets_WebApp.Enums;
using RouletteBets_WebApp.Models;

namespace RouletteBets_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetsController : ControllerBase
    {
        private readonly RouletteBetsContext _context;

        public BetsController(RouletteBetsContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IConfiguration _configuration { get; }

        //[HttpPut("{rouletteId}")]
        [HttpPut]
        [Route("CloseBets/{rouletteId}")]
        public async Task<IActionResult> CloseBets(long rouletteId)
        {
            try
            {
                bool closedRoulette = await CloseRoulette(rouletteId);
                if (!closedRoulette)
                {
                    return BadRequest();
                }
                BetResultDTO betResult = PlayRoulette();
                List<Bet> historyBets = await GetHistoryBets(rouletteId, betResult);

                return Ok(historyBets);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouletteExists(rouletteId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPost]
        [Route("CreateBet")]
        public async Task<ActionResult<Bet>> CreateBet(Bet bets)
        {
            bool validation = await DoBetValidation(bets);
            if (validation)
            {
                _context.Database.AutoTransactionsEnabled = false;
                bets.Id = 0;
                bets.UserId = GetUserId();
                bets.BetTime = DateTime.UtcNow.ToString("o");
                bets.Active = true;
                bets.User = null;
                bets.Roulette = null;
                _context.Bets.Add(bets);
                await _context.SaveChangesAsync();
                _context.Database.AutoTransactionsEnabled = true;

                return Ok(bets.Id);
            }

            return BadRequest();
        }

        private bool RouletteExists(long id)
        {
            return _context.Roulette.Any(e => e.Id == id);
        }

        private async Task<bool> CloseRoulette(long rouletteId)
        {
            Roulette roulette = await _context.Roulette.FindAsync(rouletteId);
            roulette.IsOpen = false;
            _context.Entry(roulette).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouletteExists(rouletteId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        private long GetUserId()
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                string authHeader = Request.Headers["Authorization"];
                authHeader = authHeader.Replace("Bearer ", "");
                var securityToken = tokenHandler.ReadToken(authHeader) as JwtSecurityToken;
                var stringClaimValue = securityToken.Claims.First(claim => claim.Type == "UserId").Value;

                return Convert.ToInt64(stringClaimValue);
            }
            catch (Exception)
            {
                return Convert.ToInt64(1);
            }
        }

        private BetValidationsDTO GetBetValidationsValues()
        {
            var configSection =_configuration.GetSection("BetValidation");
            BetValidationsDTO betValidation = new BetValidationsDTO
            {
                MinNumber = configSection.GetValue<long>("MinNumber"),
                MaxNumber = configSection.GetValue<long>("MaxNumber"),
                MaxMoney = configSection.GetValue<long>("MaxMoney"),
            };

            return betValidation;
        }

        private async Task<bool> DoBetValidation(Bet betData)
        {
            bool isOpen = betData.RouletteId > 0 ? await IsOpenRoulette(betData.RouletteId) : false;
            if (!isOpen)
            {
                return false;
            }
            var betValidations = GetBetValidationsValues();
            var haveCredit = await CreditMoneyValidation(betData.Money);
            if (!betData.Number.HasValue && !betData.Color.HasValue)
            {
                return false;
            }
            else if (betData.Number.HasValue && (betData.Number.Value % 2 == 0) && betData.Color != BetColorsEnum.Red)
            {
                return false;
            }
            else if (betData.Number.HasValue && (betData.Number.Value % 2 != 0) && betData.Color != BetColorsEnum.Black)
            {
                return false;
            }
            else if (betData.Number.HasValue && (betData.Number > betValidations.MaxNumber || betData.Number < betValidations.MinNumber))
            {
                return false;
            }
            else if (betData.Money > betValidations.MaxMoney || betData.Money <= 0)
            {
                return false;
            }
            else if (!haveCredit)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> CreditMoneyValidation(long betAmount)
        {
            long userId = GetUserId();
            User user = await _context.User.FindAsync(userId);
            if (user != null && user.CreditMoney > betAmount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<bool> IsOpenRoulette(long rouletteId)
        {
            var roulette = await _context.Roulette.FindAsync(rouletteId);
            if (roulette.IsOpen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private BetResultDTO PlayRoulette()
        {
            BetValidationsDTO betValidations = GetBetValidationsValues();
            Random random = new Random();
            int wonNumber = random.Next((int)betValidations.MinNumber, (int)betValidations.MaxNumber);
            BetResultDTO betResult = new BetResultDTO
            {
                Number = wonNumber,
                Color = (wonNumber % 2) == 0 ? BetColorsEnum.Red : BetColorsEnum.Black
            };

            return betResult; 
        }

        private async Task<List<Bet>> GetHistoryBets(long rouletteId, BetResultDTO betResult)
        {
            try
            {
                var betsByRoulette = await _context.Bets.Where(x => x.RouletteId == rouletteId && x.Active == true).ToListAsync();
                foreach (var bet in betsByRoulette)
                {
                    bet.Active = false;
                    if (bet.Number.HasValue && bet.Number.Value == betResult.Number)
                    {
                        bet.WonMoney = bet.Money * MoneyProfitPercentEnum.NumericBet;
                    }
                    else if (!bet.WonMoney.HasValue && bet.Color.HasValue && bet.Color.Value == betResult.Color)
                    {
                        bet.WonMoney = bet.Money * MoneyProfitPercentEnum.ColorBet;
                    }
                    else
                    {
                        bet.WonMoney = 0;
                    }
                    _context.Entry(bet).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();

                return betsByRoulette;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
