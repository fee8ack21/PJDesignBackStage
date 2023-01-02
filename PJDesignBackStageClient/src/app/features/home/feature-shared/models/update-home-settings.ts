export class HomeSettings {
  id: number;
  type: number;
  isEnabled: boolean;
  data = new HomeSettingData();

  constructor(id: number, type?: number, isEnabled?: boolean) {
    this.id = id;
    this.type = type ?? 1;
    this.isEnabled = isEnabled ?? true;
  }
}

export class HomeSettingData {
  title?: string;
  titleColor?= '#000000';
  engTitle?: string;
  engTitleColor?= '#000000';

  text?: string;
  textColor?= '#000000';

  imageName?: string;
  imageUrl?: string;

  backgroundImageName?: string;
  backgroundImageUrl?: string;
  backgroundColor?= '#ffffff';
  btnText?: string;
  btnTextColor?= '#ffffff';
  btnBorderColor?= '#ffffff';
  btnUrl?: string;

  smallIconName1?: string;
  smallIconUrl1?: string;
  smallTitle1?: string;
  smallTitle1Color?= '#000000';
  smallText1?: string;
  smallText1Color?= '#000000';

  smallIconName2?: string;
  smallIconUrl2?: string;
  smallTitle2?: string;
  smallTitle2Color?= '#000000';
  smallText2?: string;
  smallText2Color?= '#000000';

  smallIconName3?: string;
  smallIconUrl3?: string;
  smallTitle3?: string;
  smallTitle3Color?= '#000000';
  smallText3?: string;
  smallText3Color?= '#000000';

  smallIconName4?: string;
  smallIconUrl4?: string;
  smallTitle4?: string;
  smallTitle4Color?= '#000000';
  smallText4?: string;
  smallText4Color?= '#000000';

  unitId?: number;
}

export const dataBaseProperties = [
  'title',
  'titleColor',
  'text',
  'textColor'
];

export const homeType1and2DataProperties = [
  ...dataBaseProperties,
  'imageName',
  'imageUrl',
  'backgroundImageName',
  'backgroundImageUrl'
]

export const homeType3and4DataProperties = [
  ...dataBaseProperties,
  'backgroundImageName',
  'backgroundImageUrl',
  'btnText',
  'btnUrl',
  'btnTextColor',
  'btnBorderColor'
]

export const homeType5DataProperties = [
  ...dataBaseProperties
]

export const homeType6DataProperties = [
  'backgroundColor',
  'smallTitle1',
  'smallTitle1Color',
  'smallText1',
  'smallText1Color',

  'smallTitle2',
  'smallTitle2Color',
  'smallText2',
  'smallText2Color',

  'smallTitle3',
  'smallTitle3Color',
  'smallText3',
  'smallText3Color',

  'smallTitle4',
  'smallTitle4Color',
  'smallText4',
  'smallText4Color'
]

export const homeType7DataProperties = [
  'backgroundColor',
  'title',
  'titleColor',
  'engTitle',
  'engTitleColor',

  'smallIconName1',
  'smallIconUrl1',
  'smallTitle1',
  'smallTitle1Color',
  'smallText1',
  'smallText1Color',

  'smallIconName2',
  'smallIconUrl2',
  'smallTitle2',
  'smallTitle2Color',
  'smallText2',
  'smallText2Color',

  'smallIconName3',
  'smallIconUrl3',
  'smallTitle3',
  'smallTitle3Color',
  'smallText3',
  'smallText3Color',

  'smallIconName4',
  'smallIconUrl4',
  'smallTitle4',
  'smallTitle4Color',
  'smallText4',
  'smallText4Color',
]

export const homeType8and9and10DataProperties = [
  'unitId',
  'backgroundColor',
  'titleColor',
  'engTitleColor',
]
