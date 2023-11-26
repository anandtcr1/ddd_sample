using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public string EntityId { get; }
        public string EntityName { get; }

        /// <summary>
        /// Exception for entity not found, name (entityName) and optional id (entityId) . 
        /// </summary>
        public EntityNotFoundException(string name, string id)
            : base($"Entity {name}{(!string.IsNullOrEmpty(id) ? " (" + id + ")" : "")} was not found")
        {
            EntityName = name;
            EntityId = id;
        }

        /// <summary>
        /// Exception for entity not found
        /// </summary>
        /// <param name="message">localized message with two parameter {entityName:0} {entityId:1}</param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public EntityNotFoundException(string message, string name, string id) : base(string.Format(message, name, id))
        {
            EntityName = name;
            EntityId = id;
        }

        /// <summary>
        /// Exception for entity not found
        /// </summary>
        /// <param name="message">localized message with two parameter {entityName:0} {entityId:1}</param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public EntityNotFoundException(string message, string name, long id) : base(string.Format(message, name, id))
        {
            EntityName = name;
            EntityId = id.ToString();
        }

        /// <summary>
        /// Exception for entity not found
        /// </summary>
        /// <param name="message">localized message with two parameter {entityName:0} {entityId:1}</param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        public EntityNotFoundException(string message, string name, Guid id) : base(string.Format(message, name, id))
        {
            EntityName = name;
            EntityId = id.ToString();
        }


        public EntityNotFoundException(string message) : base(message)
        {
            EntityId = string.Empty;
            EntityName = string.Empty;
        }
    }
}
