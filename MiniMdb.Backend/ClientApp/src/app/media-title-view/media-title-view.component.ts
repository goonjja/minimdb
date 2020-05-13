import { Component, OnInit } from '@angular/core';
import { MediaTitlesService } from '../services/media-titles.service';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { MediaTitle, MediaTitleTypeName } from '../models/MediaTitles';
import { ApiMessage } from '../models/ApiMessage';
import { map } from 'rxjs/operators';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

@Component({
  selector: 'app-media-title-view',
  templateUrl: './media-title-view.component.html',
  styleUrls: ['./media-title-view.component.scss']
})
export class MediaTitleViewComponent implements OnInit {
  mediaTitle$: Observable<MediaTitle>;
  titleId: number;
  isAuthenticated = false;
  titleTypes;

  constructor(private titleService: MediaTitlesService, private avRoute: ActivatedRoute, private authService: AuthorizeService) {
    this.titleTypes = MediaTitleTypeName;
    const idParam = 'id';
    if(this.avRoute.snapshot.params[idParam]) {
      this.titleId = this.avRoute.snapshot.params[idParam];
    }
    this.isAuthenticated = authService.isAuthenticated();
  }

  ngOnInit(): void {
    this.loadMediaTitle();
  }

  loadMediaTitle() {
    this.mediaTitle$ = this.titleService.getMediaTitle(this.titleId).pipe(map(x => {
      if(x.data != null && x.data.length > 0) { return x.data[0]; }
      return null;
    }));
  }
}
