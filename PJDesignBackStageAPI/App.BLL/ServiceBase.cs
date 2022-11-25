using App.EF.DbContexts;

namespace App.BLL
{
    public abstract class ServiceBase
    {
        public PjdesignContext PJDesignContext()
        {
            return new PjdesignContext();
        }
    }
}