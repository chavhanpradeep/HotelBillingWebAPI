using DAL.Repositories.Interfaces;

namespace DAL.DTOs
{
    public class Config : IConfig
    {
        public string SavedPDFPath { get; set; }
    }
}