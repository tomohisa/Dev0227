import StudentsClient from './students-client';
import { getStudents } from '@/lib/server/services/students';

export default async function StudentsPage() {
  const students = await getStudents();
  return <StudentsClient initialStudents={students} />;
}
