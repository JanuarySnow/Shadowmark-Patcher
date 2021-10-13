using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using Noggog;
using System.Drawing;

namespace shadowmark_patcher
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "shadowmark_patcher.esp")
                .Run(args);
        }
        public static readonly ModKey ModKey = ModKey.FromNameAndExtension("Readable_Shadowmarks.esp");

        // activators from readable shadowmarks
        public static readonly FormLink<IActivatorGetter> ShadowMarkCache = new FormLink<IActivatorGetter>(ModKey.MakeFormKey(0x005900));
        public static readonly FormLink<IActivatorGetter> ShadowMarkDanger = new FormLink<IActivatorGetter>(ModKey.MakeFormKey(0x005901));
        public static readonly FormLink<IActivatorGetter> ShadowMarkEmpty = new FormLink<IActivatorGetter>(ModKey.MakeFormKey(0x005902));
        public static readonly FormLink<IActivatorGetter> ShadowMarkEscape = new FormLink<IActivatorGetter>(ModKey.MakeFormKey(0x005903));
        public static readonly FormLink<IActivatorGetter> ShadowMarkFence = new FormLink<IActivatorGetter>(ModKey.MakeFormKey(0x005904));
        public static readonly FormLink<IActivatorGetter> ShadowMarkGuildMember = new FormLink<IActivatorGetter>(ModKey.MakeFormKey(0x005905));
        public static readonly FormLink<IActivatorGetter> ShadowMarkLoot = new FormLink<IActivatorGetter>(ModKey.MakeFormKey(0x005906));
        public static readonly FormLink<IActivatorGetter> ShadowMarkProtected = new FormLink<IActivatorGetter>(ModKey.MakeFormKey(0x005907));
        public static readonly FormLink<IActivatorGetter> ShadowMarkSafe = new FormLink<IActivatorGetter>(ModKey.MakeFormKey(0x005908));

        // textures from placed in vanilla or mod overwrites
        public static readonly FormLink<ITextureSetGetter> ShadowMarkDangerStone01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkDangerStone01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkSafeStone01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkSafeStone01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkEscapeStone01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkEscapeStone01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkGuildMemberStone01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkGuildMemberStone01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkProtectedStone01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkProtectedStone01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkFenceStone01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkFenceStone01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkCacheStone01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkCacheStone01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkLootStone01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkLootStone01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkEmptyStone01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkEmptyStone01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkDangerWood01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkDangerWood01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkEmptyWood01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkEmptyWood01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkCacheWood01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkCacheWood01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkEscapeWood01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkEscapeWood01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkFenceWood01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkFenceWood01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkGuildMemberWood01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkGuildMemberWood01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkLootWood01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkLootWood01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkProtectedWood01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkProtectedWood01.FormKey);
        public static readonly FormLink<ITextureSetGetter> ShadowMarkSafeWood01 = new FormLink<ITextureSetGetter>(Skyrim.TextureSet.ShadowMarkSafeWood01.FormKey);

        private static readonly Dictionary<FormKey, FormKey> mark_to_activator = new()
        {
            { Skyrim.TextureSet.ShadowMarkDangerStone01.FormKey, ModKey.MakeFormKey(0x005901) },
            { Skyrim.TextureSet.ShadowMarkSafeStone01.FormKey, ModKey.MakeFormKey(0x005908) },
            { Skyrim.TextureSet.ShadowMarkEscapeStone01.FormKey, ModKey.MakeFormKey(0x005903) },
            { Skyrim.TextureSet.ShadowMarkGuildMemberStone01.FormKey, ModKey.MakeFormKey(0x005905) },
            { Skyrim.TextureSet.ShadowMarkProtectedStone01.FormKey, ModKey.MakeFormKey(0x005907) },
            { Skyrim.TextureSet.ShadowMarkFenceStone01.FormKey, ModKey.MakeFormKey(0x005904) },
            { Skyrim.TextureSet.ShadowMarkCacheStone01.FormKey, ModKey.MakeFormKey(0x005900) },
            { Skyrim.TextureSet.ShadowMarkLootStone01.FormKey, ModKey.MakeFormKey(0x005906) },
            { Skyrim.TextureSet.ShadowMarkEmptyStone01.FormKey, ModKey.MakeFormKey(0x005902) },
            { Skyrim.TextureSet.ShadowMarkDangerWood01.FormKey, ModKey.MakeFormKey(0x005901) },
            { Skyrim.TextureSet.ShadowMarkEmptyWood01.FormKey, ModKey.MakeFormKey(0x005902) },
            { Skyrim.TextureSet.ShadowMarkCacheWood01.FormKey, ModKey.MakeFormKey(0x005900) },
            { Skyrim.TextureSet.ShadowMarkEscapeWood01.FormKey, ModKey.MakeFormKey(0x005903) },
            { Skyrim.TextureSet.ShadowMarkFenceWood01.FormKey, ModKey.MakeFormKey(0x005908) },
            { Skyrim.TextureSet.ShadowMarkGuildMemberWood01.FormKey, ModKey.MakeFormKey(0x005908) },
            { Skyrim.TextureSet.ShadowMarkLootWood01.FormKey, ModKey.MakeFormKey(0x005908) },
            { Skyrim.TextureSet.ShadowMarkProtectedWood01.FormKey, ModKey.MakeFormKey(0x005908) },
            { Skyrim.TextureSet.ShadowMarkSafeWood01.FormKey, ModKey.MakeFormKey(0x005908) }
    };

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            if (!state.LoadOrder.ContainsKey(ModKey))
            {
                throw new Exception("You need readable shadowmarks mod installed for this patch to do anything.");
            }

            if (state.LoadOrder.ContainsKey(ModKey))
            {
                Console.WriteLine("Readable Shadowmarks detected. Patching load order...");
                foreach (var placedobjectgetter in state.LoadOrder.PriorityOrder.OnlyEnabled().PlacedObject().WinningContextOverrides(state.LinkCache))
                {
                    //Find the default activators and disable them ( they are based on vanilla locations )
                    //If already disabled, skip
                    if (placedobjectgetter.Record.MajorRecordFlagsRaw == 0x0000_0800) continue;
                    if (placedobjectgetter.Record.EditorID == null) 
                    {
                        // if its an activator, check if its one we want to disable
                        if (placedobjectgetter.Record.Base.TryResolve<IActivatorGetter>(state.LinkCache, out var placedObjectBase))
                        {
                            if (placedObjectBase.EditorID == "ShadowMarkCache" || placedObjectBase.EditorID == "ShadowMarkDanger" || placedObjectBase.EditorID == "ShadowMarkEmpty" || placedObjectBase.EditorID == "ShadowMarkEscape" || placedObjectBase.EditorID == "ShadowMarkFence" ||
                                placedObjectBase.EditorID == "ShadowMarkGuildMember" || placedObjectBase.EditorID == "ShadowMarkLoot" || placedObjectBase.EditorID == "ShadowMarkProtected" || placedObjectBase.EditorID == "ShadowMarkSafe")
                            {
                                Console.WriteLine("disabled" + placedObjectBase.EditorID);
                                IPlacedObject modifiedObject = placedobjectgetter.GetOrAddAsOverride(state.PatchMod);
                                modifiedObject.MajorRecordFlagsRaw |= 0x0000_0800;
                            }
                        }
                        // if its a textureset, check if its a shadowmark one
                        if (placedobjectgetter.Record.Base.TryResolve<ITextureSetGetter>(state.LinkCache, out var placedObjectBase2))
                        {
                            if (placedObjectBase2.EditorID == "ShadowMarkDangerStone01" || placedObjectBase2.EditorID == "ShadowMarkSafeStone01" || placedObjectBase2.EditorID == "ShadowMarkEscapeStone01" || placedObjectBase2.EditorID == "ShadowMarkGuildMemberStone01" ||
                                placedObjectBase2.EditorID == "ShadowMarkProtectedStone01" || placedObjectBase2.EditorID == "ShadowMarkFenceStone01" || placedObjectBase2.EditorID == "ShadowMarkCacheStone01" || placedObjectBase2.EditorID == "ShadowMarkLootStone01" ||
                                placedObjectBase2.EditorID == "ShadowMarkEmptyStone01" || placedObjectBase2.EditorID == "ShadowMarkDangerWood01" || placedObjectBase2.EditorID == "ShadowMarkEmptyWood01" || placedObjectBase2.EditorID == "ShadowMarkCacheWood01" ||
                                placedObjectBase2.EditorID == "ShadowMarkEscapeWood01" || placedObjectBase2.EditorID == "ShadowMarkFenceWood01" || placedObjectBase2.EditorID == "ShadowMarkGuildMemberWood01" || placedObjectBase2.EditorID == "ShadowMarkLootWood01" ||
                                placedObjectBase2.EditorID == "ShadowMarkProtectedWood01" || placedObjectBase2.EditorID == "ShadowMarkSafeWood01" )
                            {
                                // it is a shadowmark, now add an activator there too
                                if (placedobjectgetter.TryGetParentContext<ICell, ICellGetter>(out var cellContext))
                                {
                                    Console.WriteLine($"Processing {cellContext.Record.Name}");
                                    // Does the hard work to copy
                                    var settableCell = cellContext.GetOrAddAsOverride(state.PatchMod);
                                    // Make a new object originating from the patch mod
                                    var obj = new PlacedObject(state.PatchMod);
                                    if (mark_to_activator.TryGetValue(placedobjectgetter.Record.Base.FormKey, out var ReplacerForm))
                                    {
                                        settableCell.Temporary.Add(obj);
                                        obj.Base.SetTo(ReplacerForm);
                                        var new_primitive = new PlacedPrimitive();
                                        var new_bounds = new P3Float(40, 40, 40);
                                        var new_prim_color = Color.FromArgb(204, 76, 51);
                                        var new_placement = new Placement();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                        new_placement.Position = placedobjectgetter.Record.Placement.Position;
                                        new_placement.Rotation = placedobjectgetter.Record.Placement.Rotation;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                        new_primitive.Bounds = new_bounds;
                                        new_primitive.Color = new_prim_color;
                                        new_primitive.Unknown = 0.15f;
                                        new_primitive.Type = PlacedPrimitive.TypeEnum.Sphere;
                                        obj.Primitive = new_primitive;
                                        obj.Placement = new_placement;
                                        obj.CollisionLayer = 15;
                                        //Console.WriteLine($"Processed posx of {obj.Placement.Position.X}");
                                        //Console.WriteLine($"Processed posy of {obj.Placement.Position.X}");
                                        //Console.WriteLine($"Processed posz of {obj.Placement.Position.X}");

                                    }
                                    else
                                    {
                                        Console.WriteLine($"Error finding activator equivalent of shadowmark { placedobjectgetter.Record}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Error finding parent cell of  { placedobjectgetter.Record}");
                                }
                            }
                        }
                    }
                    
                };
                Console.WriteLine("Patch completed!");
            }
        }
    }
}