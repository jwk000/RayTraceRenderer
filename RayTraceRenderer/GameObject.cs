using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace RayTraceRenderer
{
    internal class GameObject :Behavior
    {
        List<IComponent> mComponents = new List<IComponent>();
        public void AddComponent(IComponent com)
        {
            foreach (var c in mComponents)
            {
                if (c.GetType() == com.GetType())
                {
                    return;
                }
            }

            mComponents.Add(com);
            com.gameObject = this;
        }

        public void RemoveComponent(IComponent com)
        {
            foreach (var c in mComponents)
            {
                if (c.GetType() == com.GetType())
                {
                    mComponents.Remove(c);
                    break;
                }
            }
        }

        public T? GetComponent<T>() where T : class, IComponent
        {
            foreach (IComponent c in mComponents)
            {
                if (c.GetType().IsAssignableTo(typeof(T)))
                {
                    return c as T;
                }
            }
            return null;
        }

        public GameObject Parent = null;
        public List<GameObject> Children = new List<GameObject>();

        public override void Awake()
        {
            foreach(var c in mComponents)
            {
                var b = c as Behavior;
                b.Awake();
            }
            foreach(var o in Children)
            {
                o.Awake();
            }
        }

        public override void Start()
        {
            foreach (var c in mComponents)
            {
                var b = c as Behavior;
                b.Start();
            }
            foreach (var o in Children)
            {
                o.Start();
            }
        }

        public override void Update()
        {
            foreach (var c in mComponents)
            {
                var b = c as Behavior;
                b.Update();
            }
            foreach (var o in Children)
            {
                o.Update();
            }
        }

        public override void Destroy()
        {
            foreach (var c in mComponents)
            {
                var b = c as Behavior;
                b.Destroy();
            }
            foreach (var o in Children)
            {
                o.Destroy();
            }
        }
    }

    interface IComponent
    {
        GameObject gameObject { get; set; }
    }

    abstract class AComponent :Behavior, IComponent
    {
        public GameObject gameObject { get; set; }
    }


}
