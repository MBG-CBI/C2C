export class ApplicationError {
    name: string;
    message: string;
    constructor(public errors: string[]) {}
}
