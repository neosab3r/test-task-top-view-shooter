using System.Linq;
using BeeGood.Extensions;
using BeeGood.Models;
using BeeGood.Views;
using UnityEngine;

namespace BeeGood.Systems
{
    public class BulletSystem : BaseSystem<BulletModel, BulletView>
    {
        public override bool HasUpdate() => true;

        public override void Initialize()
        {
        }

        public override BulletModel AddView(BulletView view)
        {
            var bulletModel = new BulletModel(view);
            Models.Add(bulletModel);
            return bulletModel;
        }

        public BulletModel GetBulletModelByView(BulletView view)
        {
            foreach (var model in Models.Where(model => model.View == view))
            {
                return model;
            }
            
            Debug.LogError($"No founded {nameof(BulletModel)} for {nameof(BulletView)} with name {view.name}");
            return default;
        }

        public BulletModel CreateBulletView(BulletView bulletPrefab, Transform startPosition, IModel ownerPlayer)
        {
            var bullet = InstantiateExtension.Instantiate(bulletPrefab, startPosition.position, startPosition.rotation);
            var model = AddView(bullet);
            model.OwnerPlayer = ownerPlayer;

            return model;
        }

        public override void Update(float dt)
        {
            for (var i = Models.Count - 1; i >= 0; i--)
            {
                var model = Models[i];
                if (model.ReadyToDestroy)
                {
                    model.DestroyBulletView();
                    model.Dispose();
                    RemoveModel(model);
                    continue;
                }
                
                model.TryMove();
            }
        }
    }
}