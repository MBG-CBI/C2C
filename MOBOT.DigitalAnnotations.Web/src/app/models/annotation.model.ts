import { AnnotationTarget } from './annotation-target.model';
import { DisplayModes } from './display-modes.enum';
import { License } from './license.model';
import { AnnotationType } from './annotation-type.model';
import { Tag } from './tag.model';

export class Annotation {
    public id = 0;
    public subject: string;
    public body: string;
    public createdUserId: number;
    public createdUser: string;
    public createdDate: Date;
    public updatedUserId?: number;
    public updatedUser?: string;
    public updateDate?: Date;
    public groupId: number;
    public groupName: string;
    public licenseId?: number;
    public license: License;
    public annotationTypeId?: number;
    public annotationType: AnnotationType;
    public parentId?: number;
    public annotations: Annotation[] = [];
    public tags: Tag[] = [];

    public isSelected: boolean;

    public mode: DisplayModes = DisplayModes.Create;
    public targetId: number;
    public target: AnnotationTarget = new AnnotationTarget();
    constructor(mode?: DisplayModes) {
        this.mode = mode || DisplayModes.Create;
        this.isSelected = false;
    }
}
