using Domain.Exceptions;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers
{
    [Authorize]
    public abstract class Controller : ControllerBase
    {
        protected readonly ILogger _logger;
        protected readonly IUnitOfWork _unitOfWork;

        protected Controller(ILogger logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        #region Protected methods

        protected async Task<ActionResult<TResult>> ProcessRequestAsync<TResult>(Func<Task<ActionResult<TResult>>> action, CancellationToken cancellationToken)
        {
            try
            {
                return await action();
            }
            catch (BusinessRuleException ex)
            {
                return ProcessException(ex, cancellationToken);
            }
            catch (NotFoundException ex)
            {
                return ProcessException(ex, cancellationToken);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ProcessException(ex, cancellationToken);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is BusinessRuleException businessRuleException)
                    return ProcessException(businessRuleException, cancellationToken);

                if (ex.InnerException is UnauthorizedAccessException unauthorizedAccessException)
                    return ProcessException(unauthorizedAccessException, cancellationToken);

                return ProcessException(ex, cancellationToken);
            }
        }

        protected async Task<IActionResult> ProcessRequestAsync(Func<Task<IActionResult>> action, CancellationToken cancellationToken)
        {
            try
            {
                return await action();
            }
            catch (BusinessRuleException ex)
            {
                return ProcessException(ex, cancellationToken);
            }
            catch (NotFoundException ex)
            {
                return ProcessException(ex, cancellationToken);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ProcessException(ex, cancellationToken);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is BusinessRuleException businessRuleException)
                    return ProcessException(businessRuleException, cancellationToken);

                if (ex.InnerException is UnauthorizedAccessException unauthorizedAccessException)
                    return ProcessException(unauthorizedAccessException, cancellationToken);

                return ProcessException(ex, cancellationToken);
            }
        }

        #endregion Protected methods

        #region Private methods

        private BadRequestObjectResult ProcessException(BusinessRuleException ex, CancellationToken cancellationToken)
        {
            _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogWarning(ex, "Bad request");
            return BadRequest(ex.Message);
        }

        private NotFoundResult ProcessException(NotFoundException ex, CancellationToken cancellationToken)
        {
            _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogDebug(ex, "Not found");
            return NotFound();
        }

        private UnauthorizedObjectResult ProcessException(UnauthorizedAccessException ex, CancellationToken cancellationToken)
        {
            _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogInformation(ex, "Unauthorized request");
            return Unauthorized(ex.Message);
        }

        private ObjectResult ProcessException(Exception ex, CancellationToken cancellationToken)
        {
            _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Internal server error - Unhandled exception.");
            return Problem(detail: "Internal server error", statusCode: StatusCodes.Status500InternalServerError);
        }

        #endregion Private methods
    }
}
