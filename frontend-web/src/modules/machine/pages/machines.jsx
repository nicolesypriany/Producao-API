import Header from "@/modules/core/components/header";
import { useCreateMachine } from "@/modules/machine/hooks/useCreateMachine";
import { Flex } from "@mantine/core";
import { AddMachine } from "../components/add-machine";
import TableMachines from "../components/table";

const MachinesPage = () => {
  return (
    <Flex direction="column">
      <Flex>
        <Header>Listagem de máquinas</Header>
        <AddMachine />
      </Flex>
      <TableMachines />
    </Flex>
  );
};

export default MachinesPage;
