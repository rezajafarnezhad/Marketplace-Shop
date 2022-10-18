using ProShop.Entities;

namespace ProShop.Services.Contracts;

public interface IConsignmentItemService : IGenericService<ConsignmentItem>
{
    Task<bool> IsExistsByProductVariantIdAndConsignmentId(long productvariantId, long consignmentId);
}
