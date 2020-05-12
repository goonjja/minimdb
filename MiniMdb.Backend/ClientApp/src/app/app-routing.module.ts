import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MediaTitlesComponent } from './media-titles/media-titles.component';
import { MediaTitleViewComponent } from './media-title-view/media-title-view.component';
import { MediaTitleAddEditComponent } from './media-title-add-edit/media-title-add-edit.component';


const routes: Routes = [
  { path: '', component: MediaTitlesComponent, pathMatch: 'full' },
  { path: 'title/:id', component: MediaTitleViewComponent },
  { path: 'add-title', component: MediaTitleAddEditComponent },
  { path: 'title/:id/edit', component: MediaTitleAddEditComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
