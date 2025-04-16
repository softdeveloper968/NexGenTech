using System.Net;

namespace MedHelpAuthorizations.Application.Common.Exceptions;
public class ConflictException : CustomException
{
    /// <summary>
    /// added summary
    /// </summary>
    /// <param name="message"></param>
    public ConflictException(string message)
        : base(message, null, HttpStatusCode.Conflict)
    {
    }
}