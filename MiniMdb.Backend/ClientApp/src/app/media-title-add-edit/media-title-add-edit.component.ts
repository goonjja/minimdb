import { Component, OnInit } from '@angular/core';
import { MediaTitle, MediaTitleType } from '../models/MediaTitles';
import { Observable } from 'rxjs';
import { MediaTitlesService } from '../services/media-titles.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-media-title-add-edit',
  templateUrl: './media-title-add-edit.component.html',
  styleUrls: ['./media-title-add-edit.component.scss']
})
export class MediaTitleAddEditComponent implements OnInit {
  mediaTitle: MediaTitle;
  titleId: number;
  existing: boolean;

  form: FormGroup;
  formType = 'type';
  formName = 'name';
  formPlot = 'plot';

  constructor
  (
    private titleService: MediaTitlesService,
    private formBuilder: FormBuilder,
    private avRoute: ActivatedRoute,
    private router: Router
  ) {
    const idParam = 'id';
    if (this.avRoute.snapshot.params[idParam]) {
      this.titleId = this.avRoute.snapshot.params[idParam];
    }

    this.form = this.formBuilder.group(
      {
        id: 0,
        type: [MediaTitleType.Movie],
        name: ['', [Validators.required]],
        plot: ['', [Validators.required]],
      }
    )
  }

  ngOnInit(): void {
    if(this.titleId > 0) {
      this.existing = true;
      this.loadMediaTitle();
    }
  }

  loadMediaTitle() {
    this.titleService.getMediaTitle(this.titleId).subscribe(x => {
      if(x.data != null && x.data.length > 0) {
        this.mediaTitle = x.data[0];
        this.form.controls[this.formType].setValue(this.mediaTitle.type);
        this.form.controls[this.formName].setValue(this.mediaTitle.name);
        this.form.controls[this.formPlot].setValue(this.mediaTitle.plot);
      } else {
        // todo display error
      }
    });
  }

  save() {
    if (!this.form.valid) {
      return;
    }

    const mediaTitle: MediaTitle = {
      id: +this.titleId,
      type: +this.form.get(this.formType).value,
      name: this.form.get(this.formName).value,
      plot: this.form.get(this.formPlot).value,
      releaseDate: 0
    };
    if (this.existing) {
      this.titleService.updateTitle(mediaTitle).subscribe(d => this.router.navigate([this.router.url]));
    } else {
      this.titleService.saveTitle(mediaTitle).subscribe(d => this.router.navigate([this.router.url]));
    }

    console.log(this.form);
  }

  get type() { return this.form.get(this.formType); }
  get name() { return this.form.get(this.formName); }
  get plot() { return this.form.get(this.formPlot); }
}
