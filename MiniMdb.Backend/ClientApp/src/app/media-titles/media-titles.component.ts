import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable } from '@angular/material/table';
import { MediaTitlesDataSource } from './media-titles-datasource';
import { MediaTitlesService } from '../services/media-titles.service';
import { MediaTitle, MediaTitleType } from '../models/MediaTitles';
import { tap } from 'rxjs/operators';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-media-titles',
  templateUrl: './media-titles.component.html',
  styleUrls: ['./media-titles.component.scss']
})
export class MediaTitlesComponent implements AfterViewInit, OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatTable) table: MatTable<MediaTitle>;

  nameFilter = new FormControl('');
  typeFilter = new FormControl(null);

  dataSource: MediaTitlesDataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['ops', 'id', 'name', 'type', 'plot'];

  constructor(private titlesService: MediaTitlesService) {

  }

  ngOnInit() {
    this.dataSource = new MediaTitlesDataSource(this.titlesService);
    this.dataSource.loadTitles();
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.table.dataSource = this.dataSource;

    this.paginator.page.pipe(tap(() => this.loadPage())).subscribe();
  }

  loadPage() {
    this.dataSource.loadTitles(
        this.nameFilter.value,
        this.typeFilter.value,
        this.paginator.pageIndex + 1,
        this.paginator.pageSize
    );
  }

  search() {
    this.dataSource.loadTitles(
      this.nameFilter.value,
      this.typeFilter.value,
      1,
      this.paginator.pageSize
  );
  }

  remove(titleId: number) {
    this.titlesService.removeTitle(titleId).subscribe(() => this.loadPage());
  }
}
