export enum MediaTitleType {
  Movie = 0,
  Series = 1
}

export class MediaTitle {
  id?: number;
  type: MediaTitleType;
  name: string;
  plot: string;
  releaseDate: number;
}


