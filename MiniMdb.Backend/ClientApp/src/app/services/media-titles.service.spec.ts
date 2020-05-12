import { TestBed } from '@angular/core/testing';

import { MediaTitlesService } from './media-titles.service';

describe('MediaTitlesService', () => {
  let service: MediaTitlesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MediaTitlesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
