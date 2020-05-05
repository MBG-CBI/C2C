import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AnnotationFilterType, AnnotationFilterTypes, AnnotationFilterRequest } from '../../models';
import { AnnotationFilterService, AnnotationService, AnnotationSourceService } from '../../services';

@Component({
  selector: 'app-annotation-filter',
  templateUrl: './annotation-filter.component.html',
  styleUrls: ['./annotation-filter.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AnnotationFilterComponent implements OnInit {

  public filterTypes: AnnotationFilterType[] = [];
  public filters: AnnotationFilterRequest[] = [];
  public isFiltered = false;
  public get hasFilters(): boolean {
    return this.filters.length > 0;
  }

  public searchText = '';

  constructor(private _service: AnnotationFilterService,
    private _annotationSourceService: AnnotationSourceService,
    private _annotationService: AnnotationService) { }

  ngOnInit(): void {
    this._annotationSourceService.annotationSource.subscribe(data => {
      this._service.getList(data.id)
        .subscribe(resp => {
          this.filterTypes = [...resp];
        });
    });
    // this._service.getList(this._annotationService.annotationSource.id)
    //   .subscribe(data => {
    //     this.filterTypes = [...data];
    //   });
  }

  addOrRemoveFilter(filterType: AnnotationFilterTypes, filterId: number, value: string, checked: boolean) {
    let idx = -1;
    if (filterType === AnnotationFilterTypes.Date) {
      idx = this.filters.findIndex(f => f.filterType === filterType && f.date.getTime() === new Date(value).getTime());
    } else {
      idx = this.filters.findIndex(f => f.filterType === filterType && f.filterId === filterId);
    }
    if (checked) {
      if (idx < 0) {
        const request = new AnnotationFilterRequest();
        request.filterId = filterId;
        request.filterType = filterType;
        request.date = filterType === AnnotationFilterTypes.Date ? new Date(value) : null;
        this.filters.push(request);
      }
    } else {
        this.filters.splice(idx, 1);
    }
  }

  applyFilters(): void {
    this._annotationService.applyFilters(this.searchText, [...this.filters]);
    this.isFiltered = true;
  }

  search(): void {
    this._annotationService.search(this.searchText.trim());
  }

  clearFilters(): void {
    // this.personFilters.splice(0, this.personFilters.length);
    this._annotationService.clearFilters();
    this.isFiltered = false;
  }

}
