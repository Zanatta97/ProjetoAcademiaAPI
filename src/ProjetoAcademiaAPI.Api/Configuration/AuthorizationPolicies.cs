namespace ProjetoAcademiaAPI.Api.Configuration
{
    public static class AuthorizationPolicies
    {
        //Define acesso apenas para usuários com a role "Admin"
        public const string AdminOnly = "AdminOnly";
    }
}