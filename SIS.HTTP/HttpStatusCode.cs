namespace SIS.HTTP
{
    public enum HttpStatusCode
    {
        Ok = 200,
        MovedPermamently = 301,
        TemporaryRedirect = 307,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
        NotImplemented = 501,

    }
}
