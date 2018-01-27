namespace MVCForumAutomation
{
    // TODO: move to TestAutomationEssentials!
    public abstract class AbstractBuilder<TThis, TEntity, TOwner>
        where TThis : AbstractBuilder<TThis, TEntity, TOwner>
        where TOwner : ICreator<TThis, TEntity, TOwner>
    {
        private readonly TOwner _owner;

        protected AbstractBuilder(TOwner owner)
        {
            _owner = owner;
        }

        public TThis With
        {
            get { return (TThis)this; }
        }

        public TThis And
        {
            get { return (TThis)this; }
        }

        public TEntity Go()
        {
            return _owner.Create((TThis)this);
        }
    }

    public interface ICreator<in TBuilder, out TEntity, TOwner>
        where TBuilder : AbstractBuilder<TBuilder, TEntity, TOwner>
        where TOwner : ICreator<TBuilder, TEntity, TOwner>
    {
        TEntity Create(TBuilder builder);
    }
}