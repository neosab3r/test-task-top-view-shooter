using BeeGood.Extensions;
using BeeGood.Models;
using BeeGood.Views;
using UnityEngine;

namespace BeeGood.Systems
{
    public class BulletSystem : BaseSystem<BulletModel, BulletView>
    {
        public override bool HasUpdate() => true;

        public override void Initialize() { }

        public override BulletModel AddView(BulletView view)
        {
            var bulletModel = new BulletModel(view);
            Models.Add(bulletModel);
            return bulletModel;
        }

        public void CreateBulletView(BulletView bulletPrefab, Transform startTransform, string tagEntity, IModel ownerPlayer)
        {
            var bullet = InstantiateExtension.Instantiate(bulletPrefab, startTransform.position, startTransform.rotation);
            var model = AddView(bullet);
            model.OwnerPlayer = ownerPlayer;
            model.TargetTagEntity = tagEntity;
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