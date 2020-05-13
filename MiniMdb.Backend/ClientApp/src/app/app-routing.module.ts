import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MediaTitlesComponent } from './media-titles/media-titles.component';
import { MediaTitleViewComponent } from './media-title-view/media-title-view.component';
import { MediaTitleAddEditComponent } from './media-title-add-edit/media-title-add-edit.component';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { LoginComponent } from 'src/api-authorization/login/login.component';
import { LogoutComponent } from 'src/api-authorization/logout/logout.component';


const routes: Routes = [
  { path: '', component: MediaTitlesComponent, pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent },
  { path: 'title/:id', component: MediaTitleViewComponent, canActivate: [AuthorizeGuard] },
  { path: 'add-title', component: MediaTitleAddEditComponent, canActivate: [AuthorizeGuard] },
  { path: 'title/:id/edit', component: MediaTitleAddEditComponent, canActivate: [AuthorizeGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
