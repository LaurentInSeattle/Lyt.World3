﻿namespace Lyt.World3.Messaging;

public sealed record class ToolbarCommandMessage(
    ToolbarCommandMessage.ToolbarCommand Command, object? CommandParameter = null)
{
    public enum ToolbarCommand
    {
        // Left - Main toolbar in Shell view 
        Today,
        Collection,
        Settings,
        About,

        // Right - Main toolbar in Shell view  
        ToTray, 
        Close,

        // Gallery toolbar
        GallerySetWallpaper,
        GallerySaveToDesktop,
        AddToCollection,        // Gallery Only

        // Collection toolbars 
        CollectionSetWallpaper,
        CollectionSaveToDesktop,
        RemoveFromCollection,   // Collection Only

        // Settings toolbars 
        Cleanup,
    }
}
