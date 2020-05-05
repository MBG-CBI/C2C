import { AnnotationTarget } from './annotation-target.model';
export class AnnotationSource {
    public id = 0;
    public sourceUrl: string;
    public rerumStorageUrl: string;
    public imageWidth?: number;
    public imageHeight?: number;
    public createdUser: string;
    public createdDate: Date;
    public updatedUser: string;
    public updateDate?: Date;
    public targets: Array<AnnotationTarget> = [];

    constructor() {}
}
