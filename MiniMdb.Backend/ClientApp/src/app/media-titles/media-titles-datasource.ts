import { DataSource } from '@angular/cdk/collections';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { map, catchError, finalize } from 'rxjs/operators';
import { Observable, of as observableOf, merge, BehaviorSubject, of } from 'rxjs';
import { MediaTitlesService } from '../services/media-titles.service';
import { MediaTitle } from '../models/MediaTitles';
import { ApiMessage } from '../models/ApiMessage';

/**
 * Data source for the MediaTitles view. This class should
 * encapsulate all logic for fetching and manipulating the displayed data
 * (including sorting, pagination, and filtering).
 */
export class MediaTitlesDataSource extends DataSource<MediaTitle> {
  private data = new BehaviorSubject<MediaTitle[]>([]); // = EXAMPLE_DATA;
  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSubject.asObservable();

  paginator: MatPaginator;
  sort: MatSort;
  count: number;

  constructor(private titlesService: MediaTitlesService) {
    super();
  }

  /**
   * Connect this data source to the table. The table will only update when
   * the returned stream emits new items.
   * @returns A stream of the items to be rendered.
   */
  connect(): Observable<MediaTitle[]> {
    return this.data.asObservable();
  }

  /**
   *  Called when the table is being destroyed. Use this function, to clean up
   * any open connections or free any held resources that were set up during connect.
   */
  disconnect() {
    this.data.complete();
    this.loadingSubject.complete();
  }

  loadTitles(page = 1, pageSize = 5) {
    this.loadingSubject.next(true);

    this.titlesService.getMediaTitles(page, pageSize).pipe(
      catchError(() => of([])),
      finalize(() => this.loadingSubject.next(false))
    ).subscribe(
      d => {
        const dataPage = d as ApiMessage<MediaTitle>;
        this.data.next(dataPage.data);
        this.count = dataPage.pagination?.count ?? 0;
      }
    );
  }
}
