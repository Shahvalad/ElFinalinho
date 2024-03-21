namespace Projecto.Application.Common.Helpers
{
    public static class Helper
    {
        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_" + Guid.NewGuid().ToString().Substring(0, 8)
                   + Path.GetExtension(fileName);
        }
    }
}
