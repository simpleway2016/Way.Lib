declare class FileBrowser {
    rootElement: HTMLElement;
    container: HTMLElement;
    loadingElement: HTMLElement;
    fileElement: HTMLElement;
    serverController: any;
    initdata: boolean;
    parentid: number;
    uploading: boolean;
    onSelectFile: (filepath: string) => any;
    constructor();
    updateFile(): void;
    addFile(filename: any): void;
    addFolderDialog(): void;
    additem(name: string, isFolder: boolean, imgPath?: string): HTMLElement;
    itemContextMenu(e: PointerEvent): void;
    itemClick(e: MouseEvent): void;
    rename(ele: any, id: any): void;
    deleteFile(ele: any, id: any): void;
    showLoading(): void;
    hideLoading(): void;
    show(): void;
    hide(): void;
    loadImages(parentid: any): void;
}
