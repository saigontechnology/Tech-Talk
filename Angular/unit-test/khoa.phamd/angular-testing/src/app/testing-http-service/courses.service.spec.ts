import {CoursesService} from './courses.service';
import { TestBed, fakeAsync, flush } from '@angular/core/testing';
import {HttpClientTestingModule, HttpTestingController} from '@angular/common/http/testing';
import {COURSES, findLessonsForCourse} from '@server/db-data';
import {Course} from './model';
import {HttpErrorResponse} from '@angular/common/http';

describe('Testing HTTP Service - CoursesService', () => {

    let coursesService: CoursesService,
        httpTestingController: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule],
            providers: [CoursesService]
        });

        coursesService = TestBed.inject(CoursesService);
        httpTestingController = TestBed.inject(HttpTestingController);
    });

    it('should retrieve all courses', fakeAsync(() => {
        let coursesResult;

        coursesService.findAllCourses()
            .subscribe(courses => {
                coursesResult = courses;
            });

        const req = httpTestingController.expectOne('/api/courses');

        expect(req.request.method).toEqual("GET");

        req.flush({payload: Object.values(COURSES)});
        
        expect(coursesResult).toBeTruthy('No courses returned');

        expect(coursesResult.length).toBe(12,
            "incorrect number of courses");

        const course = coursesResult.find(course => course.id == 12);

        expect(course.titles.description).toBe(
            "Angular Testing Course");
    }));

    it('should find a course by id', fakeAsync(() => {
        let courseResult;
        coursesService.findCourseById(12)
            .subscribe(course => {
                courseResult = course;
            });

        const req = httpTestingController.expectOne('/api/courses/12');

        expect(req.request.method).toEqual("GET");

        req.flush(COURSES[12]);

        expect(courseResult).toBeTruthy();
        expect(courseResult.id).toBe(12);
    }));

    it('should save the course data', fakeAsync(() => {
        let courseResult;
        const changes :Partial<Course> =
            {titles:{description: 'Testing Course'}};

        coursesService.saveCourse(12, changes)
            .subscribe(course => {
                courseResult = course;
            });

        const req = httpTestingController.expectOne('/api/courses/12');

        expect(req.request.method).toEqual("PUT");

        expect(req.request.body.titles.description)
            .toEqual(changes.titles.description);

        req.flush({
            ...COURSES[12],
            ...changes
        })

        expect(courseResult.id).toBe(12);
    }));

    it('should give an error if save course fails', fakeAsync(() => {
        let errorResult;

        const changes :Partial<Course> =
            {titles:{description: 'Testing Course'}};

        coursesService.saveCourse(12, changes)
            .subscribe(
                () => fail("the save course operation should have failed"),

                (error: HttpErrorResponse) => { errorResult = error; }
            );

        const req = httpTestingController.expectOne('/api/courses/12');

        expect(req.request.method).toEqual("PUT");

        req.flush('Save course failed', {status:500,
            statusText:'Internal Server Error'});
        
        expect(errorResult.status).toBe(500);
    }));

    it('should find a list of lessons', () => {

        coursesService.findLessons(12)
            .subscribe(lessons => {

                expect(lessons).toBeTruthy();

                expect(lessons.length).toBe(3);

            });

        const req = httpTestingController.expectOne(
            req => req.url == '/api/lessons');

        expect(req.request.method).toEqual("GET");
        expect(req.request.params.get("courseId")).toEqual("12");
        expect(req.request.params.get("filter")).toEqual("");
        expect(req.request.params.get("sortOrder")).toEqual("asc");
        expect(req.request.params.get("pageNumber")).toEqual("0");
        expect(req.request.params.get("pageSize")).toEqual("3");

        req.flush({
            payload: findLessonsForCourse(12).slice(0,3)
        });


    });

    afterEach(() => {

        httpTestingController.verify();
    });

});















