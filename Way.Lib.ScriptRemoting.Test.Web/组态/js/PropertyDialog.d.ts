declare class PropertyDialog {
    rootElement: HTMLElement;
    control: EditorControl;
    static CHKNumber: number;
    constructor(control: EditorControl);
    private setChkItemEvent(ele);
    private setImgItemEvent(ele);
    private setRootEvent();
    private setInputEvent(input);
    hide(): void;
    show(): void;
}
