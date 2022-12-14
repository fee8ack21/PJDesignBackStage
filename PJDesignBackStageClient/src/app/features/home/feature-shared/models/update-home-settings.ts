export class UpdateHomeSettingsRequest {

}

class HomeText {
    title: string;
    text: string;
    backgroundImageUrl: string;
    imageUrl: string;
    type: number;
}

class HomeUnit {
    sort: number;
    unitId: number; // type2 & portfolio
    type: number; // 1 左圖右文 2.右圖左文 3.三項目
}

