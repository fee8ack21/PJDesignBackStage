import { TemplateType } from "src/app/shared/models/enums";

export class CreateOrUpdateUnitRequest {
    id?: number;
    parent?: number;
    name: string;
    templateType: TemplateType;
    url?: string;
    isAnotherWindow: boolean;
    isEnabled: boolean;
}
