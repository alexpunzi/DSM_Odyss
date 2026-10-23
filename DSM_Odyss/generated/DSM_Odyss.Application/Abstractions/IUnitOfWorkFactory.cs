namespace DSM_Odyss.Application.Abstractions;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();
}