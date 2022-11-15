using ProShop.Entities;
using ProShop.ViewModels.Consignments;

namespace ProShop.Services.Contracts;

public interface IConsignmentService : IGenericService<Consignment>
{
    Task<Consignment> GetConsignmentForConfirmation(long consignmentId);
    Task<ShowConsignmentsViewModel> GetConsignments(ShowConsignmentsViewModel model);
    Task<ShowConsignmentDetailsViewModel> GetConsignmentDetails(long consignmentId);
    Task<Consignment> GetConsignmentToReceive(long consignmentId);
    Task<bool> IsExistsConsignmetWithReceivedStatus(long consignmentId);
    Task<Consignment> GetConsignmentWithReceivedStatus(long consignmentId);
    Task<bool> CanAddStockForConsignmentItems(long consignmentId);
}
