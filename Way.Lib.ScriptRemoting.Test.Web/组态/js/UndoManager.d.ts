declare class UndoManager {
    items: UndoItem[];
    position: number;
    enable: boolean;
    constructor();
    addUndo(undoObj: UndoItem): void;
    undo(): void;
    redo(): void;
}
declare class UndoItem {
    editor: Editor;
    enable: boolean;
    constructor(_editor: Editor);
    isSame(obj: UndoItem): boolean;
    undo(): void;
    redo(): void;
}
declare class UndoChangeProperty extends UndoItem {
    control: EditorControl;
    proName: string;
    originalValue: any;
    newValue: any;
    constructor(_editor: Editor, _control: EditorControl, proname: string, newValue: any);
    isSame(undoObj: UndoItem): boolean;
    undo(): void;
    redo(): void;
}
declare class UndoAddControl extends UndoItem {
    control: EditorControl;
    constructor(_editor: Editor, _control: EditorControl);
    undo(): void;
    redo(): void;
}
declare class UndoRemoveControls extends UndoItem {
    controls: EditorControl[];
    constructor(_editor: Editor, _controls: EditorControl[]);
    undo(): void;
    redo(): void;
}
declare class UndoMoveControls extends UndoItem {
    controls: EditorControl[];
    rects: any[];
    endRects: any[];
    constructor(_editor: Editor, _controls: EditorControl[]);
    isSame(undoObj: UndoItem): boolean;
    moveFinish(): void;
    undo(): void;
    redo(): void;
}
declare class UndoMoveControlsLayerUp extends UndoItem {
    controls: EditorControl[];
    constructor(_editor: Editor, _controls: EditorControl[]);
    undo(): void;
    redo(): void;
}
declare class UndoMoveControlsLayerDown extends UndoItem {
    controls: EditorControl[];
    constructor(_editor: Editor, _controls: EditorControl[]);
    undo(): void;
    redo(): void;
}
declare class UndoMoveControlsLayerFront extends UndoItem {
    controls: EditorControl[];
    nextEles: any[];
    constructor(_editor: Editor, _controls: EditorControl[]);
    undo(): void;
    redo(): void;
}
declare class UndoMoveControlsLayerBottom extends UndoItem {
    controls: EditorControl[];
    nextEles: any[];
    constructor(_editor: Editor, _controls: EditorControl[]);
    undo(): void;
    redo(): void;
}
declare class UndoChangeLinePoint extends UndoItem {
    control: LineControl;
    xname: string;
    yname: string;
    oldvalueX: string;
    oldvalueY: string;
    newvalueX: string;
    newvalueY: string;
    constructor(_editor: Editor, _control: LineControl, _xname: string, _yname: string);
    moveFinish(): void;
    undo(): void;
    redo(): void;
}
declare class UndoGroup extends UndoItem {
    controls: EditorControl[];
    groupCtrl: FreeGroupControl;
    constructor(_editor: Editor, _controls: EditorControl[]);
    undo(): void;
    redo(): void;
}
declare class UndoUnGroup extends UndoItem {
    groups: any[];
    constructor(_editor: Editor, _controls: FreeGroupControl[]);
    undo(): void;
    redo(): void;
}
declare class UndoPaste extends UndoItem {
    copyItems: any[];
    controls: any[];
    isSameWindow: boolean;
    constructor(_editor: Editor, _copyItems: any[], isSameWindow: boolean);
    undo(): void;
    redo(): void;
}
