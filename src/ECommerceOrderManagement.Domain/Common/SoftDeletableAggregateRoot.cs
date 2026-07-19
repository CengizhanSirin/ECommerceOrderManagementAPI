namespace ECommerceOrderManagement.Domain.Common
{
    public abstract class SoftDeletableAggregateRoot : AggregateRoot
    {
        public bool IsDeleted { get; private set; }

        public DateTime? DeletedAtUtc { get; private set; }

        protected SoftDeletableAggregateRoot()
        {
        }

        protected SoftDeletableAggregateRoot(Guid id) : base(id)
        {
        }

        public void Delete(DateTime deletedAtUtc)
        {
            if (IsDeleted)
            {
                return;
            }

            IsDeleted = true;
            DeletedAtUtc = deletedAtUtc;
        }

        public void Restore()
        {
            if (!IsDeleted)
            {
                return;
            }

            IsDeleted = false;
            DeletedAtUtc = null;
        }
    }
}
