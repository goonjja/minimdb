import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaTitleViewComponent } from './media-title-view.component';

describe('MediaTitleViewComponent', () => {
  let component: MediaTitleViewComponent;
  let fixture: ComponentFixture<MediaTitleViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaTitleViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaTitleViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
