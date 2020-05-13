export enum MediaTitleType {
  Movie = 0,
  Series = 1
}

export const MediaTitleTypeName = new Map<number, string>([
  [MediaTitleType.Movie, 'Movie'],
  [MediaTitleType.Series, 'Series']
]);

export class MediaTitle {
  id?: number;
  type: MediaTitleType;
  name: string;
  plot: string;
  releaseDate: number;
}
