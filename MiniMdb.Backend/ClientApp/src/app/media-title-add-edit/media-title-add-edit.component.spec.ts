import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaTitleAddEditComponent } from './media-title-add-edit.component';

describe('MediaTitleAddEditComponent', () => {
  let component: MediaTitleAddEditComponent;
  let fixture: ComponentFixture<MediaTitleAddEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaTitleAddEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaTitleAddEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
