"use client"

import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { LOCATIONS } from "@/constants/reservation"

interface LocationSelectorProps {
  label: string
  value: string
  onChange: (value: string) => void
  id: string
}

export function LocationSelector({ label, value, onChange, id }: LocationSelectorProps) {
  return (
    <div className="space-y-2">
      <Label htmlFor={id}>{label}</Label>
      <Select value={value} onValueChange={onChange}>
        <SelectTrigger id={id}>
          <SelectValue placeholder="Select location" />
        </SelectTrigger>
        <SelectContent>
          {LOCATIONS.map((location) => (
            <SelectItem key={location.id} value={location.id}>
              {location.name}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
    </div>
  )
}
