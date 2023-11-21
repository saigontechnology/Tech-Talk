export interface Lesson {
    id: number;
    description: string;
    duration: string;
    seqNo: number;
    courseId: number;
}

export interface Course {
    id: number;
    seqNo: number;
    titles: {
        description: string;
        longDescription?: string;
    };
    iconUrl: string;
    uploadedImageUrl: string;
    courseListIcon: string;
    category: string;
    lessonsCount: number;
}
