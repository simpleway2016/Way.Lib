declare function rgb(r: any, g: any, b: any): {
    "r": any;
    "g": any;
    "b": any;
};
declare class PropertyDialog {
    rootElement: HTMLElement;
    control: EditorControl;
    documentClickEvent: any;
    isShowed: boolean;
    static CHKNumber: number;
    constructor(control: EditorControl);
    private _documentElementClick;
    dispose(): void;
    private setPointItemEvent;
    private setChkItemEvent;
    private setImgItemEvent;
    private setRootEvent;
    private setInputEvent;
    hide(): void;
    show(): void;
}
