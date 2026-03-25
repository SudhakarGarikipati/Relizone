namespace CartService.Application.Dtos
{
    public class CartDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public decimal Total { get; set; }

        public decimal Tax { get;set; }

        public decimal GrandTotal { get; set; }

        public ICollection<CartItemDto> CartItems { get; set; }
    }
}
