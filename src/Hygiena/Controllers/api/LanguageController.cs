using System;
using System.Collections.Generic;
using Infrastructure.ClientEntities;
using Infrastructure.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hygiena.Controllers.api
{
    [Route("api")]
    public class LanguageController : Controller
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;

        public LanguageController(IDataService dataService, ILoggerFactory loggerFactory)
        {
            _dataService = dataService;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [Route("GetTranslation")]
        [AllowAnonymous]
        public IActionResult GetLanguageData(string language)
        {
            try
            {
                return Ok(_dataService.GetCaptionTranslation(language, User?.Identity?.Name ?? ""));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} :: GetTranslation");
                return StatusCode(500);
            }
        }

        [Route("UserLanguage")]
        [AllowAnonymous]
        public IActionResult UserLanguage()
        {
            try
            {
                return User?.Identity?.Name == null
                    ? Ok("English")
                    : Ok(_dataService.GetUserLanguage(User?.Identity?.Name));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} :: UserLanguage");
                return StatusCode(500);
            }
        }

        [Route("UpdateUserLanguage")]
        public IActionResult UpdateUserLanguage(string language)
        {
            try
            {
                _dataService.UpdateUserLanguage(User.Identity.Name, language);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} :: UpdateUserLanguage");
                return StatusCode(500);
            }
        }

        [Route("GetLogicalName")]
        [AllowAnonymous]
        public IActionResult GetLogicalName()
        {
            try
            {
                return Ok(_dataService.GetLogicalName());
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} :: GetLogicalName");
                return StatusCode(500);
            }
        }

        [Route("AddLogicalName")]
        public IActionResult AddLogicalName(string logicName)
        {
            try
            {
                if (logicName == null) return StatusCode(500);
                _dataService.AddLogicalName(logicName);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} :: AddLogicalName");
                return StatusCode(500);
            }
        }

        [Route("DeleteLogicalName")]
        public IActionResult DeleteLogicalName(string logicName)
        {
            try
            {
                if (logicName == null) return StatusCode(500);
                _dataService.DeleteLogicalName(logicName);
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} :: DeleteLogicalName");
                return StatusCode(500);
            }
        }

        [Route("UpdateLanguageData")]
        public IActionResult UpdateLanguageData(string language, IEnumerable<Translation> languageData)
        {
            try
            {
                _dataService.UpdateTranslation(language, languageData);
                return Ok("Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} :: UpdateLanguageData");
                return StatusCode(500);
            }
        }

        #region User Language Values

        [Route("GetUserLanguageDefinition")]
        public IActionResult GetUserLanguageDefinition(string language)
        {
            try
            {
                return Ok(_dataService.GetUserDefinedLanguage(User.Identity.Name, language));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} :: GetUserLanguageDefinition");
                return StatusCode(500);
            }
        }

        [Route("RemoveUserLanguage")]
        public IActionResult RemoveUserLanguage(string language, string logicName)
        {
            try
            {
                _dataService.RemoveUserLanguage(User?.Identity?.Name, language, logicName);
                return Ok("Ok");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} :: RemoveUserLanguage");
                return StatusCode(500);
            }
        }

        [Route("UpdateAddUserDefinition")]
        public IActionResult UpdateAddUserDefinition(string language, string caption, string userCaption,
            string userToolTip)
        {
            try
            {
                _dataService.UpdateUserLanguage(User?.Identity?.Name, language, caption, userCaption, userToolTip);
                return Ok("Ok");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} :: UpdateAddUserDefinition");
                return StatusCode(500);
            }
        }

        [Route("GetWordsByLanguage")]
        public IActionResult GetWordsByLanguage(string language)
        {
            try
            {
                return Ok(_dataService.GetWordsByLanguage(language));
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, $"{User?.Identity?.Name} :: GetWordsByLanguage");
                return StatusCode(500);
            }
        }

        #endregion
    }
}