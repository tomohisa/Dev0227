"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { createClass } from "./actions";
import { useRouter } from "next/navigation";

interface AddClassDialogProps {
  onSuccess?: () => void;
}

export function AddClassDialog({ onSuccess }: AddClassDialogProps) {
  const router = useRouter();
  const [open, setOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  
  const [formData, setFormData] = useState({
    name: "",
    classCode: "",
    description: "",
    capacity: "",
    location: "",
    schedule: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      setLoading(true);
      setError(null);
      
      // Validate required fields
      if (!formData.name) {
        setError("Name is required");
        setLoading(false);
        return;
      }
      
      if (!formData.classCode) {
        setError("Class code is required");
        setLoading(false);
        return;
      }
      
      if (!formData.description) {
        setError("Description is required");
        setLoading(false);
        return;
      }
      
      // Parse capacity to number
      const capacity = formData.capacity ? parseInt(formData.capacity, 10) : 0;
      if (isNaN(capacity)) {
        setError("Capacity must be a valid number");
        setLoading(false);
        return;
      }
      
      console.log("Submitting class data:", {
        name: formData.name,
        classCode: formData.classCode,
        description: formData.description,
        capacity: capacity,
        location: formData.location,
        schedule: formData.schedule,
      });
      
      const result = await createClass({
        name: formData.name,
        classCode: formData.classCode,
        description: formData.description,
      });
      
      if (result.error) {
        setError(result.error);
      } else {
        setOpen(false);
        setFormData({
          name: "",
          classCode: "",
          description: "",
          capacity: "",
          location: "",
          schedule: "",
        });
        
        // Call onSuccess callback if provided
        onSuccess?.();
      }
    } catch (err: any) {
      console.error("Error registering class:", err);
      setError(err.response?.data?.detail || "Failed to register class. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button>Add New Class</Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Add New Class</DialogTitle>
            <DialogDescription>
              Enter the class details below to register a new class.
            </DialogDescription>
          </DialogHeader>
          <div className="grid gap-4 py-4">
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="name" className="text-right">
                Name
              </Label>
              <Input
                id="name"
                name="name"
                value={formData.name}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="classCode" className="text-right">
                Class Code
              </Label>
              <Input
                id="classCode"
                name="classCode"
                value={formData.classCode}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="description" className="text-right">
                Description
              </Label>
              <Input
                id="description"
                name="description"
                value={formData.description}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="capacity" className="text-right">
                Capacity
              </Label>
              <Input
                id="capacity"
                name="capacity"
                type="number"
                value={formData.capacity}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="location" className="text-right">
                Location
              </Label>
              <Input
                id="location"
                name="location"
                value={formData.location}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="schedule" className="text-right">
                Schedule
              </Label>
              <Input
                id="schedule"
                name="schedule"
                value={formData.schedule}
                onChange={handleChange}
                className="col-span-3"
                required
              />
            </div>
          </div>
          {error && (
            <div className="bg-red-50 p-2 rounded-md text-red-500 mb-4">
              <p>{error}</p>
            </div>
          )}
          <DialogFooter>
            <Button type="submit" disabled={loading}>
              {loading ? "Registering..." : "Register Class"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
