namespace Icarus.API.Jobs
{
    public interface IPrintWelcomeJob
    {
        public void PrintWelcome();
        public void CleanUserTable();
        public void CleanProductTable();
    }
}
