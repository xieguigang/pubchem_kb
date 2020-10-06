declare class bootbox {
    public static prompt(message: string, handle: (input: string) => void);
    public static dialog(config: {
        title: string,
        message: string,
        buttons: {
            cancel?: bootboxButton,
            confirm: bootboxButton
        }
    });
}

interface bootboxButton {
    label: string;
    className?: string;
    callback?: Delegate.Action;
}