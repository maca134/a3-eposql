#region usings

using System;
using EpoSql.Core;
using System.Collections.Generic;

#endregion

namespace EpoSql.Model
{
    public abstract class Base
    {
        protected readonly object Locker = new object();
        private bool _isNew = true;
        protected int _expiry = -1;
        protected long _id = -1;
        protected Guid _classId = Guid.NewGuid();

        public virtual bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }
        public virtual long Id
        {
            get { return _id; }
            protected set { _id = value; }
        }
        public virtual DateTime Expiry
        {
            get
            {
                if (_expiry == -1)
                    return DateTime.MaxValue;

                var expiry = DateTime.Now;
                expiry = expiry.AddSeconds(_expiry);
                return expiry;
            }
            protected set { _expiry = (int)value.Subtract(DateTime.Now).TotalSeconds; }
        }
        public virtual DateTime Modified
        {
            get { return DateTime.Now; }
            protected set { }
        }
        public virtual Guid ClassId
        {
            get { return _classId; }
        }

        public virtual void SaveOrUpdate()
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(this);
                    transaction.Commit();
                }
            }
        }
    }
}