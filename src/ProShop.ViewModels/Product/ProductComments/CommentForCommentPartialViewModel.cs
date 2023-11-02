namespace ProShop.ViewModels.Product.ProductComments;

public class CommentForCommentPartialViewModel
{
    public int CurrentPage { get; set; }
    public List<ProductCommentForProductInfoViewModel> ProductComments { get; set; }
    public int CommentPageCount { get; set; }
    public List<LikedCommentByUserViewModel> LikedCommentByUser { get; set; } = new();
}